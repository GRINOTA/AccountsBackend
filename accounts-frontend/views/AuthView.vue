<template>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <form @submit.prevent="login">
                    <div class="mb-3">
                        <label for="inputLogin" class="form-label">Логин</label>
                        <input type="text" class="form-control" id="inputLogin" aria-describedby="loginHelp" v-model="user.login">
                    </div>
                    <div class="mb-3">
                        <label for="inputPassword" class="form-label">Пароль</label>
                        <input type="password" class="form-control" id="inputPassword" v-model="user.password">
                    </div>
                    <div class="mb-3 to_register">
                        <label class="form-check-label">У вас ещё нет аккаунта? Зарегистрироваться</label>
                    </div>
                    <button type="submit" class="btn btn-primary">Войти</button>
                </form> 
            </div>
        </div>
    </div>  
</template>

<script>
    import AuthService from '../services/authService'

    export default {
        data() {
            return {
                user: {
                    login: 'grinch',
                    password: '12345'
                }
                
            }
        },
        methods: {
            login() {
                AuthService.login(this.user).then(
                    () => {
                        this.$router.push('/')
                    },
                    error => {
                        console.log(error)
                    }
                )
            }
        }
    }
</script>