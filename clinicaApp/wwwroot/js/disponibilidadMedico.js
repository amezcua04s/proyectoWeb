document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".agregar-rango").forEach(boton => {
        boton.addEventListener("click", () => {
            const dia = boton.dataset.dia;
            const contenedor = document.getElementById(`rangos-${dia}`);

            const rangosExistentes = contenedor.querySelectorAll(".rango-horario");
            const idUnico = Date.now();

            const nuevoRango = document.createElement("div");
            nuevoRango.className = "input-group mb-2 rango-horario";

            nuevoRango.innerHTML = `
                <input type="text" id="inicio-${idUnico}" class="form-control flatpickr-time inicio" placeholder="Hora inicio" readonly />
                <span class="input-group-text">a</span>
                <input type="text" id="fin-${idUnico}" class="form-control flatpickr-time fin" placeholder="Hora fin" readonly />
                <button type="button" class="btn btn-sm btn-danger ms-2 quitar-rango">X</button>
            `;

            contenedor.insertBefore(nuevoRango, boton);

            const inputInicio = nuevoRango.querySelector(".inicio");
            const inputFin = nuevoRango.querySelector(".fin");

            // Obtener hora por defecto
            let defaultInicio = "08:00";
            if (rangosExistentes.length > 0) {
                const ultimoFin = rangosExistentes[rangosExistentes.length - 1].querySelector(".fin").value;
                if (ultimoFin) defaultInicio = ultimoFin;
            }

            flatpickr(inputInicio, {
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i",
                defaultDate: defaultInicio,
                time_24hr: true,
                allowInput: false,
                onChange: function (selectedDates) {
                    if (selectedDates.length > 0) {
                        const nuevaHora = new Date(selectedDates[0]);
                        nuevaHora.setHours(nuevaHora.getHours() + 1);
                        const horas = nuevaHora.getHours().toString().padStart(2, "0");
                        const minutos = nuevaHora.getMinutes().toString().padStart(2, "0");
                        inputFin._flatpickr.setDate(`${horas}:${minutos}`, true);
                    }
                    actualizarValores(dia);
                }
            });

            flatpickr(inputFin, {
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i",
                time_24hr: true,
                allowInput: false
            });

            inputFin.addEventListener("change", () => actualizarValores(dia));

            nuevoRango.querySelector(".quitar-rango").addEventListener("click", () => {
                nuevoRango.remove();
                actualizarValores(dia);
            });
        });
    });

    function actualizarValores(dia) {
        const rangos = document.querySelectorAll(`#rangos-${dia} .rango-horario`);
        const resultado = [];

        rangos.forEach(grupo => {
            const inicio = grupo.querySelector(".inicio").value;
            const fin = grupo.querySelector(".fin").value;

            if (inicio && fin && fin > inicio) {
                resultado.push(`${inicio}-${fin}`);
            }
        });

        const campoOculto = document.querySelector(`input[name="DisponibilidadesPorDia[${dia}]"]`);
        if (campoOculto) campoOculto.value = resultado.join(",");
    }
});
