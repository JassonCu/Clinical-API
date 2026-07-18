'use strict';

document.addEventListener('DOMContentLoaded', function () {
    var modal = document.getElementById('tokenModal');
    if (modal) new bootstrap.Modal(modal).show();

    var copyBtn = document.getElementById('copyBtn');
    if (copyBtn) copyBtn.addEventListener('click', copyToken);
});

function copyToken() {
    navigator.clipboard.writeText(document.getElementById('tokenValue').value).then(function () {
        document.getElementById('copyIcon').className = 'bi bi-clipboard-check text-success';
    });
}
