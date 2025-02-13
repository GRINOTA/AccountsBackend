import axios from "axios";

const API_URL = 'http://localhost:5244/api';

class CurrencyRatesService {
    async getRateByTargetRate(idTargetRate) {
        const responce = await axios.get(API_URL + `/CurrencyRates/${idTargetRate}`)
        return responce.data
    } 
}

export default new CurrencyRatesService