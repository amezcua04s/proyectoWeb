const saveUser = (user) => {
    // Obtiene los usuarios del localStorage
    let users = JSON.parse(localStorage.getItem('usuarios')) || [];
    // Agrega el nuevo usuario al arreglo
    users.push(user);
    localStorage.setItem('usuarios', JSON.stringify(users));
};

const getUsers = () => {
    // Obtiene y retorna los usuarios almacenados.
    return JSON.parse(localStorage.getItem('usuarios')) || [];
};