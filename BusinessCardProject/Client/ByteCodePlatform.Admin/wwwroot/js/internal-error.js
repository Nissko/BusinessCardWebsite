let countdownInterval = null;
let countdown = 15;

function startAutoReload() {
    if (countdownInterval) {
        clearInterval(countdownInterval);
    }

    countdown = 15;
    updateCountdownDisplay();

    countdownInterval = setInterval(() => {
        countdown--;
        updateCountdownDisplay();

        if (countdown <= 0) {
            clearInterval(countdownInterval);
            countdownInterval = null;
            window.location.reload();
        }
    }, 1000);
}

function updateCountdownDisplay() {
    const countdownElement = document.getElementById('countdown');
    if (countdownElement) {
        countdownElement.textContent = countdown;
    }
}

function cancelAutoReload() {
    console.log('cancelAutoReload вызвана');

    if (countdownInterval) {
        clearInterval(countdownInterval);
        countdownInterval = null;
    }

    const errorUi = document.getElementById('blazor-error-ui');
    if (errorUi) {
        errorUi.style.display = 'none';
        errorUi.classList.remove('show');
    }
}

const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
        if (mutation.type === 'attributes' && mutation.attributeName === 'style') {
            const errorUi = document.getElementById('blazor-error-ui');
            if (!errorUi) return;

            const inlineDisplay = errorUi.style.display;

            if (inlineDisplay && inlineDisplay !== 'none') {
                if (!errorUi.classList.contains('show')) {
                    errorUi.classList.add('show');
                }

                if (!countdownInterval) {
                    startAutoReload();
                }
            }
        }
    });
});

document.addEventListener('DOMContentLoaded', () => {
    const errorUi = document.getElementById('blazor-error-ui');
    if (errorUi) {
        observer.observe(errorUi, {
            attributes: true,
            attributeFilter: ['style']
        });

        const inlineDisplay = errorUi.style.display;
        if (inlineDisplay && inlineDisplay !== 'none') {
            errorUi.classList.add('show');
            startAutoReload();
        }

        const dismissButton = document.querySelector('.dismiss');
        if (dismissButton) {
            dismissButton.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation();
                cancelAutoReload();
            });
        }

        const reloadButton = document.querySelector('.reload');
        if (reloadButton) {
            reloadButton.addEventListener('click', (e) => {
                e.preventDefault();
                if (countdownInterval) {
                    clearInterval(countdownInterval);
                    countdownInterval = null;
                }
                window.location.reload();
            });
        }
    }
});