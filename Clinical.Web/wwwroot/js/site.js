'use strict';

document.addEventListener('DOMContentLoaded', () => {

    /* ─── Auto-dismiss alerts ─── */
    document.querySelectorAll('.alert-floating').forEach(el => {
        setTimeout(() => {
            el.classList.remove('show');
            setTimeout(() => el.remove(), 300);
        }, 4000);
    });

    /* ─── Sidebar toggle for mobile ─── */
    const sidebar = document.getElementById('sidebar');
    const toggleBtn = document.getElementById('sidebar-toggle');
    if (toggleBtn && sidebar) {
        toggleBtn.addEventListener('click', () => sidebar.classList.toggle('open'));
        document.addEventListener('click', e => {
            if (sidebar.classList.contains('open') && !sidebar.contains(e.target) && !toggleBtn.contains(e.target))
                sidebar.classList.remove('open');
        });
    }

    /* ─── DataTable init ─── */
    if (typeof $.fn !== 'undefined' && typeof $.fn.DataTable !== 'undefined') {
        document.querySelectorAll('table[data-datatable]').forEach(table => {
            $(table).DataTable({
                language: { url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json' },
                pageLength: 15,
                responsive: true,
                order: []
            });
        });
    }

    /* ─── Confirm dialog con Bootstrap modal (sin SweetAlert2 / sin estilos inline) ─── */
    const confirmModalEl = document.getElementById('confirmModal');
    const confirmBtn     = document.getElementById('confirmBtn');
    const confirmMessage = document.getElementById('confirmMessage');

    if (confirmModalEl && confirmBtn) {
        const bsModal = new bootstrap.Modal(confirmModalEl);
        let pendingForm = null;

        document.querySelectorAll('[data-confirm]').forEach(btn => {
            btn.addEventListener('click', e => {
                e.preventDefault();
                confirmMessage.textContent = btn.dataset.confirm || '¿Estás seguro?';
                pendingForm = btn.closest('form') || document.getElementById(btn.dataset.form);
                bsModal.show();
            });
        });

        confirmBtn.addEventListener('click', () => {
            bsModal.hide();
            if (pendingForm) pendingForm.submit();
        });

        confirmModalEl.addEventListener('hidden.bs.modal', () => { pendingForm = null; });
    }

    /* ─── Prescription detail: eliminar fila (event delegation, sin onclick inline) ─── */
    const prescriptionContainer = document.getElementById('prescription-details');
    if (prescriptionContainer) {
        prescriptionContainer.addEventListener('click', e => {
            const btn = e.target.closest('[data-remove-row]');
            if (btn) btn.closest('.detail-row').remove();
        });
    }

    /* ─── Prescription: allergy check before saving ─── */
    const rxForm = document.getElementById('prescription-form');
    if (rxForm) {
        rxForm.addEventListener('submit', async e => {
            // A prior check already warned/confirmed → let this submit through.
            if (rxForm.dataset.allergyChecked === 'true') return;

            const patientId = parseInt(rxForm.querySelector('[name="PatientId"]')?.value, 10);
            const medicineIds = Array.from(rxForm.querySelectorAll('select[name$=".MedicineId"]'))
                .map(s => parseInt(s.value, 10))
                .filter(v => Number.isInteger(v) && v > 0);

            // Nothing to check → normal submit.
            if (!Number.isInteger(patientId) || medicineIds.length === 0) return;

            e.preventDefault();

            let result = null;
            try {
                const resp = await fetch(rxForm.dataset.checkUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ patientId, medicineIds })
                });
                if (resp.ok) result = await resp.json();
            } catch { /* on failure, don't block saving */ }

            if (result && result.hasConflicts) {
                renderAllergyWarning(document.getElementById('allergy-warning'), result.conflicts);
                rxForm.dataset.allergyChecked = 'true'; // next click is an explicit "save anyway"
                const btn = rxForm.querySelector('button[type="submit"]');
                if (btn) btn.innerHTML = '<i class="bi bi-exclamation-triangle me-2"></i>Guardar de todas formas';
            } else {
                rxForm.dataset.allergyChecked = 'true';
                rxForm.requestSubmit();
            }
        });
    }

});

function renderAllergyWarning(box, conflicts) {
    if (!box) return;
    const items = (conflicts || []).map(c =>
        `<li><strong>${escapeHtml(c.medicineName)}</strong> — alergia registrada a <strong>${escapeHtml(c.allergenName)}</strong>` +
        (c.severity ? ` (severidad: ${escapeHtml(c.severity)})` : '') +
        (c.reaction ? ` · reacción: ${escapeHtml(c.reaction)}` : '') +
        `</li>`).join('');
    box.innerHTML =
        `<div class="alert alert-danger">
           <div class="fw-bold mb-1"><i class="bi bi-exclamation-octagon me-2"></i>Alerta de alergias</div>
           <p class="small mb-2">El paciente tiene alergias que coinciden con medicamentos de esta receta. Revisá antes de continuar.</p>
           <ul class="small mb-0">${items}</ul>
         </div>`;
    box.scrollIntoView({ behavior: 'smooth', block: 'center' });
}

function escapeHtml(value) {
    return String(value ?? '').replace(/[&<>"']/g, c =>
        ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
}

/* ─── Prescription detail: agregar fila ─── */
window.addPrescriptionRow = function (medicines) {
    const container = document.getElementById('prescription-details');
    if (!container) return;
    const idx = container.querySelectorAll('.detail-row').length;
    const row = document.createElement('div');
    row.className = 'detail-row row g-2 mb-2 align-items-end';
    row.innerHTML = `
      <div class="col-md-3">
        <label class="form-label">Medicamento</label>
        <select name="Details[${idx}].MedicineId" class="form-select form-select-sm" required>
          <option value="">Seleccione...</option>
          ${medicines.map(m => `<option value="${m.medicineId}">${m.name} (${m.concentration})</option>`).join('')}
        </select>
      </div>
      <div class="col-md-1">
        <label class="form-label">Cant.</label>
        <input name="Details[${idx}].Quantity" type="number" min="1" class="form-control form-control-sm" required>
      </div>
      <div class="col-md-2">
        <label class="form-label">Dosis</label>
        <input name="Details[${idx}].Dosage" class="form-control form-control-sm" placeholder="ej: 500mg" required>
      </div>
      <div class="col-md-2">
        <label class="form-label">Frecuencia</label>
        <input name="Details[${idx}].Frequency" class="form-control form-control-sm" placeholder="ej: cada 8h" required>
      </div>
      <div class="col-md-2">
        <label class="form-label">Duración</label>
        <input name="Details[${idx}].Duration" class="form-control form-control-sm" placeholder="ej: 7 días" required>
      </div>
      <div class="col-md-1">
        <label class="form-label">&nbsp;</label>
        <button type="button" class="btn btn-outline-danger btn-sm d-block" data-remove-row>
          <i class="bi bi-trash"></i>
        </button>
      </div>`;
    container.appendChild(row);
};
