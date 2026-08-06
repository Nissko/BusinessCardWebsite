const SW_VERSION = '1.0.0.1';
const swUrl = `service-worker.published.js?v=${SW_VERSION}`;

if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
        navigator.serviceWorker.register(swUrl)
            .then(registration => console.log('SW registered'))
            .catch(error => console.error('SW registration failed:', error));
    });
}

navigator.serviceWorker?.addEventListener('controllerchange', () => {
    window.location.reload();
});