/**FUNCIONES PARA NAVEGAR */
function goToInicio(ventana){

    let URL= window.location.href;

    if(URL){

        URL = URL.split("/").slice(0, -1).join("/");
        URL = URL + "/"+ventana+".html";
        window.open(URL, "_self");
    
    }

}

const doctores = [
    {
        nombre: "Dra. Laura Díaz",
        especialidad: "Anestesiólogo",
        telefono: "55 1122 3344",
        email: "dra.diaz@hospital.com",
        imagen: "anestesiologo.jpg",
        horario: "7:00 AM - 6:00 PM (Lunes a Viernes)"
    },
    {
        nombre: "Dr. Carlos Méndez",
        especialidad: "Cardiólogo",
        telefono: "55 1234 5678",
        email: "dr.mendez@hospital.com",
        imagen: "cardiologo.jpg",
        horario: "7:00 AM - 3:00 PM (Lunes-Jueves), 8:00 AM - 6:00 PM (Viernes)"
    },
    {
        nombre: "Dra. Ana López",
        especialidad: "Pediatra",
        telefono: "55 8765 4321",
        email: "dra.lopez@hospital.com",
        imagen: "pediatra.jpg",
        horario: "7:00 AM - 6:00 PM (Lunes a Sábado)"
    },
    {
        nombre: "Dr. Roberto Sánchez",
        especialidad: "Cirujano",
        telefono: "55 5566 7788",
        email: "dr.sanchez@hospital.com",
        imagen: "cirujano.jpg",
        horario: "8:00 AM - 6:00 PM (Lunes a Viernes)"
    },
    {
        nombre: "Dra. Patricia Rivera",
        especialidad: "Neurólogo",
        telefono: "55 9900 1122",
        email: "dra.rivera@hospital.com",
        imagen: "neurologo.jpg",
        horario: "7:00 AM - 2:00 PM (Lunes-Miércoles), 9:00 AM - 6:00 PM (Jueves-Viernes)"
    },
    {
        nombre: "Dr. Jorge Morales",
        especialidad: "Ortopédico",
        telefono: "55 3344 5566",
        email: "dr.morales@hospital.com",
        imagen: "ortopedico.jpg",
        horario: "7:00 AM - 6:00 PM (Martes a Sábado)"
    },
    {
        nombre: "Dra. Fernanda Castro",
        especialidad: "Urólogo",
        telefono: "55 7788 9900",
        email: "dra.castro@hospital.com",
        imagen: "urologo.jpg",
        horario: "7:30 AM - 6:00 PM (Lunes a Viernes)"
    },
    {
        nombre: "Dr. Ricardo Vargas",
        especialidad: "Psiquiatra",
        telefono: "55 2233 4455",
        email: "dr.vargas@hospital.com",
        imagen: "psiquiatra.jpg",
        horario: "9:00 AM - 6:00 PM (Lunes a Viernes)"
    }
];

function renderDoctores() {
    const container = document.getElementById('catalogoDoctores');
    container.innerHTML = '';

    doctores.forEach(doctor => {
        const doctorHTML = `
        <div class="col" data-especialidad="${doctor.especialidad}">
            <div class="card h-100 doctor-card shadow-sm">
                <img src="img/doctores/${doctor.imagen}" class="card-img-top" alt="${doctor.especialidad}">
                <div class="card-body">
                    <h5 class="card-title">${doctor.nombre}</h5>
                    <p class="card-text text-muted mb-1"><i class="bi bi-briefcase me-2"></i>${doctor.especialidad}</p>
                    <div class="info-adicional">
                        <hr>
                        <p><i class="bi bi-clock"></i>${doctor.horario}</p>
                        <p><i class="bi bi-telephone"></i>${doctor.telefono}</p>
                        <p><i class="bi bi-envelope"></i>${doctor.email}</p>
                    </div>
                </div>
            </div>
        </div>
        `;
        container.insertAdjacentHTML('beforeend', doctorHTML);
    });
}


function filtrarDoctores() {
    const especialidad = document.getElementById('filtroEspecialidad').value;
    
    document.querySelectorAll('#catalogoDoctores .col').forEach(doctor => {
        if (especialidad === 'todas' || doctor.dataset.especialidad === especialidad) {
            doctor.style.display = 'block';
        } else {
            doctor.style.display = 'none';
        }
    });
}

document.addEventListener('DOMContentLoaded', () => {
    renderDoctores(); 
    
    
    document.getElementById('filtroEspecialidad').addEventListener('change', filtrarDoctores);
    
    
    document.addEventListener('click', function(e) {
        if (window.innerWidth <= 768 && e.target.closest('.doctor-card')) {
            e.target.closest('.doctor-card').classList.toggle('active');
        }
    });
});

