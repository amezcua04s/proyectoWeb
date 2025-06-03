document.addEventListener("DOMContentLoaded", function () {
    const dias = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];

    dias.forEach(dia => {
        const input = document.getElementById(`input-${dia}`);
        const toggle = document.getElementById(`toggle-${dia}`);
        const contenedor = document.getElementById(`contenedor-${dia}`);

        if (input) {
            flatpickr(input, {
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i",
                mode: "multiple",
                minuteIncrement: 60
            });
        }

        toggle.addEventListener("change", () => {
            contenedor.style.display = toggle.checked ? "block" : "none";
            input.disabled = !toggle.checked;
            if (!toggle.checked) input.value = "";
        });
    });
});

