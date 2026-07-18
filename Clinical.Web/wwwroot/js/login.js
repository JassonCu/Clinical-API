'use strict';

document.addEventListener('DOMContentLoaded', function () {
    var btn = document.getElementById('pwd-toggle');
    if (btn) btn.addEventListener('click', togglePwd);
});

function togglePwd() {
    var p = document.getElementById('pwd');
    var i = document.getElementById('eye-icon');
    p.type = p.type === 'password' ? 'text' : 'password';
    i.className = p.type === 'password' ? 'bi bi-eye' : 'bi bi-eye-slash';
}
