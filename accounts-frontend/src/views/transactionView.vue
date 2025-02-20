<template>
    <div class="container mt-4 d-flex justify-content-center align-items-start" style="min-height: 100vh;">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <legend class="mb-0">Перевод средств</legend>
                </div>
                <div class="card-body">
                    <form>
                        <fieldset>
                            <div class="my-3">
                                <label class="form-label">Тип перевода</label>
                                <select class="form-select" v-model="transactionType">
                                    <option value="own">Между своими</option>
                                    <option value="other">На чужой счет</option>
                                </select>
                            </div>
                            <div class="my-3">
                                <label for="disabledSelect" class="form-label">Счет отправителя</label>
                                <select id="disabledSelect" class="form-select" v-model="transaction.numberSenderAccount">
                                    <option v-for="account in selectedAccounts" :key="account.number" :value="account.number">
                                        {{account.number}}
                                    </option>
                                </select>
                            </div>

                            <div class="my-3" v-if="transactionType === 'own'">
                                <label for="disabledSelect" class="form-label">Счет получателя</label>
                                <select id="disabledSelect" class="form-select" v-model="transaction.numberRecipientAccount">
                                    <option v-for="account in filteredAccounts" :key="account.number" :value="account.number">
                                        {{account.number}}
                                    </option>
                                </select>
                            </div>

                            <div class="my-3" v-if="transactionType === 'other'">
                                <label for="disabledSelect" class="form-label" v-if="transactionType === 'other'">Счет получателя</label>
                                <input type="text" class="form-control" v-model="transaction.numberRecipientAccount">
                            </div>

                            <div class="my-3">
                                <label class="form-label">Сколько</label>
                                <input class="form-control" v-model.number="transaction.amount"/>
                            </div>
                            
                            <button type="submit" class="btn btn-primary" @click="createTransaction">Перевести</button>
                        </fieldset>
                    </form> 
                </div>
            </div> 
        </div>
    </div>
</template>

<script>
    import AccountsService from '../api/services/accountsService';
    import TransactionService from '../api/services/transactionService';
    
    export default {
        data() {
            return {
                selectedAccounts: [],
                transaction: {
                    numberSenderAccount: null, 
                    numberRecipientAccount: null, 
                    amount: null
                },
                transactionType: 'own'
            }
        },
        computed: {
            filteredAccounts() {
                return this.selectedAccounts.filter(account => account.number !== this.transaction.numberSenderAccount)
            }
        },
        async mounted() {
            this.selectedAccounts = await AccountsService.getAccountsList()
        },
        methods: {
            async createTransaction() {
                try {
                    await TransactionService.createTransaction(this.transaction)
                    this.$toast.success('Перевод выполнен')
                    this.$router.replace('/')
                } catch(error) {
                    this.$toast.error(`${error.response.status} ${error.response.data.message}`)
                }
            }
        }
    }
</script>