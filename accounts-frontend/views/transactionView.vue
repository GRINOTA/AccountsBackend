<template>
    <div>
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
    </div>
</template>

<script>
    import AccountsService from '../services/accountsService';
    
    export default {
        data() {
            return {
                selectedAccounts: [],
                numberSenderAccount: null, 
                numberRecipientAccount: null, 
                amount: null    
            }
        },
        computed: {
            filteredAccounts() {
                return this.selectedAccounts.filter(account => account.number !== this.numberSenderAccount)
            }

        },
        async mounted() {
            this.selectedAccounts = await AccountsService.getAccountsList()
        },
        methods: {
            
        }
    }
</script>