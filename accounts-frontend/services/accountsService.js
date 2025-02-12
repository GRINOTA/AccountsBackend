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
            console.log(responce.data)
        }).catch(error => {
            if(error.responce) {
                console.error("Ошибка сервера: ", error.responce.data)
                console.error("Код статуса: ", error.responce.status)
            } else if(error.request) {
                console.error("Запрос отправлен, ответ не получен: ", error.request)
            } else {
                console.error("Ошибка при настройке запроса: ", error.message)
            }
            
        })
        // return null;
    }
}

export default new AccountsService()
