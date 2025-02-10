<template>
    <div>
        <div style="display: flex;">
            <h1>Главная</h1>
            <button @click="logout" type="button" class="btn btn-outline-primary">Выйти</button>
        </div>

        <div>
            <button type="button" class="btn btn-primary">Добавить счет</button>
        </div>
        <div>
            <table class="table table-bordered border-primary">
                <thead>
                    <tr>
                        <th>Номер</th>
                        <th>Баланс</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="account in accounts" :key="account.number">
                        <td>{{account.number}}</td>
                        <td>{{account.balance}} {{account.currency}}</td>
                    </tr>
                </tbody>
            </table> 
        </div>
        
        <form>
            <fieldset>
                <legend>Перевод средств</legend>

                <div class="mb-3">
                    <label for="disabledSelect" class="form-label">Счет отправителя</label>
                    <select id="disabledSelect" class="form-select" v-model="numberSenderAccount">
                        <option v-for="account in selectedAccounts" :key="account.number" :value="account.number">
                            {{account.number}}
                        </option>
                    </select>
                </div>

                <div class="mb-3">
                    <label for="disabledSelect" class="form-label">Счет получателя</label>
                    <select id="disabledSelect" class="form-select" v-model="numberRecipientAccount">
                        <option v-for="account in filteredAccounts" :key="account.number" :value="account.number">
                            {{account.number}}
                        </option>
                    </select>
                </div>

                <div class="mb-3">
                    <label class="form-label">Сколько</label>
                    <input class="form-control" v-model.number="amount"/>
                </div>
                
                <button type="submit" class="btn btn-primary" @click="sendMoney">Перевести</button>
            </fieldset>
        </form>
        
        <div>
            <button>Отчет</button>
        </div>  
    </div>
</template>

<script>
import AccountsService from '../services/accountsService'
import AuthService from '../services/authService'
// import TransactionService from '../services/transactionService'
import axios from "axios"

export default {
    data() {
        return {
            accounts: [],
            selectedAccounts: [],
            numberSenderAccount: null, 
            numberRecipientAccount: null, 
            amount: null
        }
    },
    mounted() {
        this.getAccounts()
    },
    computed: {
        filteredAccounts() {
            return this.selectedAccounts.filter(account => account.number !== this.numberSenderAccount)
        }

    },
    methods: {
        logout() {
            AuthService.logout()
            this.$router.push('/login')
        },
        async getAccounts() {
            this.accounts = await AccountsService.getAccountsList()
            this.selectedAccounts = await AccountsService.getAccountsList()
        },
        async sendMoney() {
            // await TransactionService.createTransaction(this.numberSenderAccount, this.numberRecipientAccount, this.amount)

            const responce = await axios
                .post('http://localhost:5244/api/Transaction', {params: {numberSenderAccount: this.numberSenderAccount, numberRecipientAccount: this.numberRecipientAccount, amount: this.amount}});
            this.data = responce.data
        }
    }
}

</script>
