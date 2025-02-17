import axios from "axios";

const API_URL = 'http://localhost:5244/api';

class AccountsService {
    async getAccountsList() {
        const responce = await axios
            .get(API_URL + '/Account/users');
        return responce.data
    }

    async createAccount(idCurrency) {

        await axios.post(
            API_URL + '/Account', idCurrency,
            {headers: {'Content-Type': 'application/json'}}
        ).then(responce => {
            alert(responce.data.message)
        }).catch(error => {
            alert(error.data.message)
        })
    }
}

export default new AccountsService()
