import axios from "axios";

const API_URL = 'http://localhost:5244/api';

class CurrencyService {
    async getAllCurrency() {
        const responce = await axios
            .get(API_URL + '/Currency');

        return responce.data;
    }
}

export default new CurrencyService()