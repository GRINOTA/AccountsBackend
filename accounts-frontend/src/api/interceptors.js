import axios from 'axios'
import router from '../router'
import AuthService from './services/authService'

axios.interceptors.response.use(
    responce => responce,
    error => {
        if(error.response?.status === 401) {
            AuthService.logout()
            router.push('/login')
        }
        return Promise.reject(error)
    }
)