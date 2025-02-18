import axios from "axios"
import {API_URL} from '../api'

class CurrencyRatesService {
    async getRateByTargetRate(idTargetRate) {
        try {
            var response = await axios
                .get(`${API_URL}/CurrencyRates/${idTargetRate}`)
            return response.data
        } catch (error) {
            return Promise.reject(error)
        }
    } 
}

export default new CurrencyRatesService