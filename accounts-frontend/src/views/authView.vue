<template>
    <div class="container d-flex justify-content-center align-items-center" style="min-height: 100vh;">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <legend class="mb-0">{{isAuth ? 'Авторизация' : 'Регистрация'}}</legend>
                    </div>
                    <div class="card-body">
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
                            <label class="form-label" v-if="isAuth && unauthResponce" style="color: red;">{{errorMessage}}</label>
                            <div class="mb-3">
                                <button type="submit" class="btn btn-primary" v-if="isAuth">Войти</button>
                                <button type="submit" class="btn btn-primary" v-if="!isAuth">Зарегистрироваться</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
    </div>  
</template>

<script>
    import AuthService from '../api/services/authService'

    export default {
        data() {
            return {
                errorMessage: null,
                unauthResponce: false,
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
                try {
                    if(this.isAuth) {
                        const responce = await AuthService.login(this.user)
                        if(responce.data) {
                            localStorage.setItem('token', responce.data);
                            this.$router.replace('/')
                            this.$toast.success('Вы успешно авторизовались')
                        } else {
                            this.$toast.error(`${responce.message}`)
                        }        
                    } else {
                        if(this.userRegister.surname !== null
                            && this.userRegister.firstName !== null
                            && this.userRegister.login  !== null
                            && this.userRegister.password !== null
                        ) {
                            if(this.userRegister.password === this.passwordConfirm) {
                                await AuthService.register(this.userRegister)
                                    .then(response => {
                                        this.isAuth = true;
                                        this.$toast.success(`Регистрация прошла успешно`)
                                        return response
                                    }).catch(error => {
                                        this.$toast.error(`${error.message}`)
                                    })
                            } else {
                                this.$toast.error('Пароли не совпадают')
                            }
                        } else {
                            this.$toast.error('Поля не должны быть пустыми (кроме отчества)')    
                        }
                    }
                } catch(error) {
                    this.$toast.error(`${error.status} ${error.statusText}`) 
                }
            },
            registerAuthEvent() {
                event.preventDefault()
                this.isAuth = !this.isAuth
            }
        }
    }
</script>