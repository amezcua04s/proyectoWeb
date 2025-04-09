// Selección de elementos del formulario
const form = document.querySelector('form');
const usuarioInput = document.getElementById('usuario');
const passwordInput = document.getElementById('password');
const recordarCheckbox = document.getElementById('recordar');

// Cargar datos almacenados si "Recordar Usuario" está activado
document.addEventListener('DOMContentLoaded', () => {
    const usuarioGuardado = localStorage.getItem('usuario');
    const recordar = localStorage.getItem('recordar') === 'true';

    if (recordar && usuarioGuardado) {
        usuarioInput.value = usuarioGuardado;
        recordarCheckbox.checked = true;
    }
});

// Manejar el envío del formulario
form.addEventListener('submit', (e) => {
    e.preventDefault(); 

    const usuario = usuarioInput.value;
    const password = passwordInput.value;
    const recordar = recordarCheckbox.checked;

    if (recordar) {
        // Guarda el usuario y estado de "Recordar Usuario"
        localStorage.setItem('usuario', usuario);
        localStorage.setItem('recordar', true);
    } else {
        // Limpiar datos si "Recordar Usuario" no están activados
        localStorage.removeItem('usuario');
        localStorage.setItem('recordar', false);
    }

    // Simular inicio de sesión exitoso
    alert(`Inicio de sesión exitoso para el usuario: ${usuario}`);
    form.reset(); 
});