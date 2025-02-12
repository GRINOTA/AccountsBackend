import axios from "axios";

const API_URL = 'http://localhost:5244/api';

class AuthService {
    async login(user) {
        const responce = await axios
            .post(API_URL + '/Auth/login', user);
        if (responce.data.token) {
            localStorage.setItem('token', responce.data.token);
        }
        return responce.data;
    }

    async register(userRequest) {
        const responce = await axios.post(API_URL + '/Auth/register', userRequest);
        return responce.data
    }

    async logout() {
        localStorage.removeItem('token')
    }

    isAuthenticated() {
        return !!localStorage.getItem('token')
    }
}

export default new AuthService