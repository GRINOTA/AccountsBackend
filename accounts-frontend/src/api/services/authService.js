import axios from "axios"
import {API_URL} from '../api'

class AuthService {
    async login(user) {
        try {
            const responce = await axios
            .post(`${API_URL}/Auth/login`, user);
            if (responce.data.token) {
                localStorage.setItem('token', responce.data.token);
            }
            return responce;
        } catch(error) {
            return Promise.reject(error)
        }
    }

    async register(userRequest) {
        try {
            const responce = await axios.post(`${API_URL}/Auth/register`, userRequest);
            return responce
        } catch(error) {
            return Promise.reject(error)
        } 
    }

    async logout() {
        localStorage.removeItem('token')
    }
}

export default new AuthService