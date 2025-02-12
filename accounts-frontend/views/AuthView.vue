<template>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <form @submit.prevent="login">
                    <div class="mb-3" v-if="!isAuth">
                        <label for="inputSurname" class="form-label">Фамилия</label>
                        <input type="text" class="form-control" id="inputSurname" aria-describedby="surnameHelp" v-model="userRegister.surname">
                    </div>
                    <div class="mb-3" v-if="!isAuth">
                        <label for="inputFirstName" class="form-label">Имя</label>
                        <input type="text" class="form-control" id="inputFirstName" aria-describedby="firstNameHelp" v-model="userRegister.firstName">
                    </div>
                    <div class="mb-3" v-if="!isAuth">
                        <label for="inputMiddleName" class="form-label">Отчество</label>
                        <input type="text" class="form-control" id="inputMiddleName" aria-describedby="middleNameHelp" v-model="userRegister.middleName">
                    </div>
                    <div class="mb-3">
                        <label for="inputLogin" class="form-label">Логин</label>
                        <input type="text" class="form-control" id="inputLogin" aria-describedby="loginHelp" v-model="user.login" v-if="isAuth">
                        <input type="text" class="form-control" id="inputLogin" aria-describedby="loginHelp" v-model="userRegister.login" v-if="!isAuth">
                    </div>
                    <div class="mb-3">
                        <label for="inputPassword" class="form-label">Пароль</label>
                        <input type="password" class="form-control" id="inputPassword" v-model="user.password" v-if="isAuth">
                        <input type="password" class="form-control" id="inputPassword" v-model="userRegister.password" v-if="!isAuth">
                    </div>
                    <div class="mb-3" v-if="!isAuth">
                        <label for="inputPasswordConfirm" class="form-label">Подтвердите пароль</label>
                        <input type="password" class="form-control" id="inputPasswordConfirm" v-model="passwordConfirm">
                    </div>
                    <div class="mb-3">
                        <a type="button" href="#" @click="registerAuthEvent">
                            <label class="form-label" v-if="isAuth">У вас ещё нет аккаунта? Зарегистрироваться</label>
                            <label class="form-label" v-if="!isAuth">У вас уже есть аккаунта? Авторизоваться</label>
                        </a>
                    </div>
                    <div class="mb-3">
                        <button type="submit" class="btn btn-primary" v-if="isAuth">Войти</button>
                        <button type="submit" class="btn btn-primary" v-if="!isAuth">Зарегистрироваться</button>
                    </div>
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
                    login: null,
                    password: null
                },
                isAuth: true,
                userRegister: {
                    surname: null,
                    firstName: null,
                    middleName: null,
                    login: null,
                    password: null
                },
                passwordConfirm: null
            }
        },
        methods: {
            async login() {
                if(this.isAuth) {
                    await AuthService.login(this.user).then(
                        () => {
                            this.$router.push('/')
                        },
                        error => {
                            console.log(error)
                        }
                    )
                } else {
                    if(this.userRegister.password == this.passwordConfirm) {
                        await AuthService.register(this.userRegister)

                        window.location.reload()
                    }
                }
            },
            registerAuthEvent() {
                event.preventDefault()
                this.isAuth = !this.isAuth
            }
        }
    }
</script>