(function () {
    'use strict';
    try {
        const userSettingsJson = localStorage.getItem('userSettings') ?? "false";
        let isDark = false;

        if (userSettingsJson) {
            const userSettings = JSON.parse(userSettingsJson);
            isDark = userSettings.IsDark === true;
        } else if (window.matchMedia) {
            isDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        }

        document.documentElement.setAttribute('data-theme', isDark.toString());
    } catch (e) {
        document.documentElement.setAttribute('data-theme', 'false');
    }
})();