import axios from "axios";

const API_URL = 'http://localhost:5244/api';

class TransactionService {
    async createTransaction(transaction) {
        await axios
            .post(
                API_URL + '/Transaction', transaction
            ).then(responce => {
                console.log(responce.data)
            }).catch(error => {
                if(error.responce) {
                    console.error("Ошибка сервера: ", error.responce.data)
                    console.error("Код статуса: ", error.responce.status)
                } else if(error.requst) {
                    console.error("Запрос отправлен, ответ не получен: ", error.request)    
                } else {
                    console.error("Ошибка при настройке запроса: ", error.message)    
                } 
            });
    }
}

export default new TransactionService()