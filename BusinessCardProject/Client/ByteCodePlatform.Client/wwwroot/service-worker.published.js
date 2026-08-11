self.importScripts('./service-worker-assets.js');

const swLocation = new URL(self.location.href);
const swVersion = swLocation.searchParams.get('v') || self.assetsManifest.version || '0';

self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event, swVersion)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${swVersion}`;

const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/];
const offlineAssetsExclude = [/^service-worker\.js$/, /^service-worker\.published\.js$/];

const base = "/";
const baseUrl = new URL(base, self.origin);
const manifestUrlList = self.assetsManifest.assets.map(asset => new URL(asset.url, baseUrl).href);

async function onInstall(event) {
    const cache = await caches.open(cacheName);
    const cachedItems = await cache.keys();
    if (cachedItems.length > 0) {
        await self.skipWaiting();
        return;
    }

    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));

    try {
        await cache.addAll(assetsRequests);
    } catch (e) {
        console.error('Failed to cache some assets during install:', e);
    }

    await self.skipWaiting();
}

async function onActivate(event, currentSwVersion) {
    const metaCache = await caches.open('sw-metadata');
    const previousVersion = await metaCache.match('version').then(r => r ? r.text() : null);

    if (previousVersion && previousVersion !== currentSwVersion) {
        const allKeys = await caches.keys();
        await Promise.all(allKeys
            .filter(key => key !== 'sw-metadata')
            .map(key => caches.delete(key)));
    }

    await metaCache.put('version', new Response(currentSwVersion));
    await self.clients.claim();
}

async function onFetch(event) {
    if (event.request.method !== 'GET') return fetch(event.request);

    const shouldServeIndexHtml = event.request.mode === 'navigate'
        && !manifestUrlList.some(url => url === event.request.url);

    const request = shouldServeIndexHtml ? 'index.html' : event.request;

    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);

    return cachedResponse || fetch(event.request).catch(() => cachedResponse);
}