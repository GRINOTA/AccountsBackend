import axios from "axios"
import {API_URL} from '../api'

class TransactionService {
    async createTransaction(transaction) {
        await axios
            .post(
                `${API_URL}/Transaction`, transaction
            ).then(response => {
                console.log(response.status)
                return response
            }).catch(error => {
                return Promise.reject(error)
            }
        )
    }

    async getTransaction() {
        try {
            const responce = await axios.get(`${API_URL}/Transaction`);
            return responce.data
        } catch (error) {
            return Promise.reject(error)
        }
    }
}

export default new TransactionService()