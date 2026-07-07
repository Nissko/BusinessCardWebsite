window.themeHelper = {
    setTheme: function(isDark) {
        if (isDark) {
            document.documentElement.setAttribute('data-theme', 'true');
        } else {
            document.documentElement.setAttribute('data-theme', 'false');
        }
    }
};