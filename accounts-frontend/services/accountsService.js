import axios from "axios";

const API_URL = 'http://localhost:5244/api';

class AccountsService {
    async getAccountsList() {
        const responce = await axios
            .get(API_URL + '/Account/users');
        return responce.data
    }
}

export default new AccountsService()