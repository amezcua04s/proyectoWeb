document.addEventListener("DOMContentLoaded", function () {
    const especialidadesExistentes = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(
        Model.Especialidades.Select(e => e.ToLower()).ToList()
    ));

    const form = document.getElementById("formEspecialidad");
    const nombreInput = document.getElementById("nombreEspecialidad");

    if (!form || !nombreInput) return;

    form.addEventListener("submit", function (e) {
        const nombre = nombreInput.value.trim().toLowerCase();
        if (especialidadesExistentes.includes(nombre)) {
            e.preventDefault();
            alert("La especialidad ya existe.");
        }
    });
});