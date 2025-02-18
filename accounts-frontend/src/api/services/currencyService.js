import axios from "axios"
import {API_URL} from '../api'

class CurrencyService {
    async getAllCurrency() {
        try {
            const responce = await axios
                .get(`${API_URL}/Currency`);

            return responce.data;
        } catch(error) {
            return Promise.reject(error)
        }
    }
}

export default new CurrencyService()