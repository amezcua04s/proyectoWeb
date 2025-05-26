
const doctores = [
    {
        img: "/img/admin/juan.jpg",
        title: "Dr. Juan Pérez",
        desc: "Anestesiólogo",
        especialidad: "Anestesiólogo",
        telefono: "55 1234 5678",
        email: "j.perez@hospital.com",
        horario: "Lunes a Viernes, 7:00 - 15:00",
        experiencia: "15 años de experiencia",
        formacion: "UNAM, Especialidad en Anestesiología"
    },
    {
        img: "/img/admin/desiree.jpeg",
        title: "Dra. Desiree García",
        desc: "Cardióloga",
        especialidad: "Cardiólogo",
        telefono: "55 2345 6789",
        email: "d.garcia@hospital.com",
        horario: "Lunes a Jueves, 8:00 - 16:00",
        experiencia: "12 años de experiencia",
        formacion: "IPN, Especialidad en Cardiología"
    },
    {
        img: "/img/admin/samuel.jpeg",
        title: "Dr. Samuel López",
        desc: "Pediatra",
        especialidad: "Pediatra",
        telefono: "55 3456 7890",
        email: "s.lopez@hospital.com",
        horario: "Martes a Sábado, 9:00 - 17:00",
        experiencia: "10 años de experiencia",
        formacion: "UAM, Especialidad en Pediatría"
    },
    {
        img: "/img/admin/paola.jpeg",
        title: "Dra. Paola Vives",
        desc: "Cirujana",
        especialidad: "Cirujano",
        telefono: "55 4567 8901",
        email: "p.vives@hospital.com",
        horario: "Lunes a Viernes, 8:00 - 18:00",
        experiencia: "18 años de experiencia",
        formacion: "UNAM, Especialidad en Cirugía General"
    },
    {
        img: "/img/admin/ester.jpeg",
        title: "Dra. Ester Segura",
        desc: "Neuróloga",
        especialidad: "Neurólogo",
        telefono: "55 5678 9012",
        email: "e.segura@hospital.com",
        horario: "Miércoles a Domingo, 10:00 - 18:00",
        experiencia: "14 años de experiencia",
        formacion: "UANL, Especialidad en Neurología"
    },
    {
        img: "/img/admin/maria.jpeg",
        title: "Dra. María Consuelo",
        desc: "Ortopédica",
        especialidad: "Ortopédico",
        telefono: "55 6789 0123",
        email: "m.consuelo@hospital.com",
        horario: "Lunes a Viernes, 7:30 - 15:30",
        experiencia: "16 años de experiencia",
        formacion: "UDG, Especialidad en Ortopedia"
    },
    {
        img: "/img/admin/pascual.jpeg",
        title: "Dr. Pascual Matas",
        desc: "Cardiólogo",
        especialidad: "Cardiólogo",
        telefono: "55 7890 1234",
        email: "p.matas@hospital.com",
        horario: "Lunes a Jueves, 7:00 - 14:00",
        experiencia: "20 años de experiencia",
        formacion: "UNAM, Especialidad en Cardiología"
    },
    {
        img: "/img/admin/jorge.jpeg",
        title: "Dr. Jorge Palacios",
        desc: "Urólogo",
        especialidad: "Urólogo",
        telefono: "55 8901 2345",
        email: "j.palacios@hospital.com",
        horario: "Martes a Sábado, 8:00 - 16:00",
        experiencia: "13 años de experiencia",
        formacion: "UdeG, Especialidad en Urología"
    },
    {
        img: "/img/admin/jesus.jpeg",
        title: "Dr. Jesús Paz",
        desc: "Psiquiatra",
        especialidad: "Psiquiatra",
        telefono: "55 9012 3456",
        email: "j.paz@hospital.com",
        horario: "Lunes a Viernes, 9:00 - 17:00",
        experiencia: "11 años de experiencia",
        formacion: "UV, Especialidad en Psiquiatría"
    }
];

// 2. Renderiza las tarjetas de doctores
function renderDoctores(filtro = 'todas') {
    const container = document.getElementById("doctor-list");

    // Limpiar contenedor con transición suave
    container.style.opacity = 0;
    setTimeout(() => {
        container.innerHTML = '';

        // Filtrar y mostrar doctores
        const doctoresFiltrados = filtro === 'todas'
            ? doctores
            : doctores.filter(d => d.especialidad === filtro);

        if (doctoresFiltrados.length === 0) {
            container.innerHTML = `
                <div class="col-12 text-center py-5">
                    <div class="alert alert-info">
                        No se encontraron doctores en esta especialidad
                    </div>
                </div>
            `;
        } else {
            doctoresFiltrados.forEach(doctor => {
                const col = document.createElement("div");
                col.className = "col-md-4 col-sm-6 mb-4";
                col.innerHTML = `
                    <div class="card h-100 shadow-sm doctor-card">
                        <img src="${doctor.img}" class="card-img-top" alt="${doctor.title}">
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title">${doctor.title}</h5>
                            <p class="text-muted mb-2">
                                <i class="bi bi-briefcase me-2"></i>${doctor.desc}
                            </p>
                            <div class="mt-auto">
                                <button class="btn btn-primary w-100 ver-mas-btn" 
                                        data-doctor='${JSON.stringify(doctor).replace(/'/g, "\\'")}'>
                                    Ver más
                                </button>
                            </div>
                        </div>
                    </div>
                `;
                container.appendChild(col);
            });
        }

        container.style.opacity = 1;
    }, 300);
}

// 3. Carga los detalles del doctor en el modal
function cargarDetallesModal(doctorData) {
    const doctor = typeof doctorData === 'string'
        ? JSON.parse(doctorData)
        : doctorData;

    const modal = new bootstrap.Modal(document.getElementById('doctorModal'));
    const modalBody = document.getElementById('doctor-modal-body');

    modalBody.innerHTML = `
        <div class="row">
            <div class="col-md-4 text-center">
                <img src="${doctor.img}" class="img-fluid rounded mb-3" alt="${doctor.title}">
                <h4>${doctor.title}</h4>
                <p class="text-primary">${doctor.desc}</p>
            </div>
            <div class="col-md-8">
                <h5 class="border-bottom pb-2">Información de Contacto</h5>
                <p><i class="bi bi-telephone text-primary me-2"></i> ${doctor.telefono}</p>
                <p><i class="bi bi-envelope text-primary me-2"></i> ${doctor.email}</p>
                <p><i class="bi bi-clock text-primary me-2"></i> ${doctor.horario}</p>
                
                <h5 class="border-bottom pb-2 mt-4">Experiencia</h5>
                <p>${doctor.experiencia}</p>
                
                <h5 class="border-bottom pb-2 mt-4">Formación</h5>
                <p>${doctor.formacion}</p>
            </div>
        </div>
    `;

    modal.show();
}

// 4. Filtra los doctores por especialidad
function filtrarDoctores() {
    const especialidad = document.getElementById('filtroEspecialidad').value;
    renderDoctores(especialidad);
}

// 5. Inicialización cuando el DOM está listo
document.addEventListener('DOMContentLoaded', () => {
    // Renderizar todos los doctores inicialmente
    renderDoctores();

    // Configurar el filtro
    document.getElementById('filtroEspecialidad').addEventListener('change', filtrarDoctores);

    // Delegación de eventos para los botones "Ver más"
    document.addEventListener('click', (e) => {
        if (e.target.classList.contains('ver-mas-btn')) {
            const doctorData = e.target.getAttribute('data-doctor');
            cargarDetallesModal(doctorData);
        }
    });

    // Opcional: Inicializar tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});