import config from 'config';
import { authHeader } from '../helpers';
import axios from "axios";

export const userService = {
    login,
    logout,
    register,
    getAll,
    exportpdf
};

function login(correo, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ correo, password })
    };

    return fetch(`${config.apiUrl}/usuario/autentificar`, requestOptions)
        .then(handleResponse)
        .then(usuario => {
            if (usuario.token) {
                localStorage.setItem('user', JSON.stringify(usuario));
            }

            return usuario;
        });
}

function logout() {
    localStorage.removeItem('user');
}

function register(user) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(`${config.apiUrl}/usuario/registrar`, requestOptions).then(handleResponse);
}

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/usuario`, requestOptions).then(handleResponse);
}

function exportpdf() {

    const requestOptions = {
        headers: authHeader(),
        responseType: 'blob'
    };

    axios.get(`${config.apiUrl}/usuario/exportar`, requestOptions).then(response => {
        const fileURL = window.URL.createObjectURL(
            new Blob([response.data], { type: response.data.type })
            );

        const fileLink = document.createElement('a');

        fileLink.href = fileURL;     
        fileLink.setAttribute('download', 'usuarios.pdf');  
        document.body.appendChild(fileLink); 
        fileLink.click();
        
    }).catch(error => {
      console.log(error);
    });
}

function handleResponse(response) {

    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                logout();
                location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }
        
        return data;
    });
}