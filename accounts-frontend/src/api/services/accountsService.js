import axios from "axios"
import { API_URL } from '../api'

class AccountsService {
    async getAccountsList() {
        try {
            const response = await axios
                .get(`${API_URL}/Account/users`)
            
                return response.data
        } catch(error) {
            return Promise.reject(error)
        }
    }

    async createAccount(idCurrency) {
        try {
            var response = await axios.post(
                `${API_URL}/Account`, idCurrency,{
                    headers: {'Content-Type': 'application/json'}
                }
            )
            return response.data
        } catch(error) {
            return Promise.reject(error)
        }
    }
}

export default new AccountsService()
