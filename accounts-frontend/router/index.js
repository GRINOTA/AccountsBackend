import CreateAccount from "../views/createAccount.vue"
import AuthService from "../services/authService"
import AuthView from "../views/authView.vue"
import HomeView from "../views/homeView.vue"
import TransactionView from "../views/transactionView.vue"
import ReportView from "../views/reportView.vue"
import { createRouter, createWebHashHistory } from "vue-router"


const routes = [
    {path: '/login', component: AuthView},
    {
        path: '/', 
        // beforeEnter: (to, from, next) => {

        //     // выполнить проверку аутентификации пользователя
        //     if(!AuthService.isAuthenticated()) {
        //         next('/login')
        //     }
            
        // },
        component: HomeView
    },
    {path: '/create', component: CreateAccount},
    {path: '/transaction', component: TransactionView},
    {path: '/report', component: ReportView},
]

const router = createRouter({history: createWebHashHistory(), routes})

router.beforeEach((to, from, next) => {
    const isAuthenticated = AuthService.isAuthenticated

    if(to.meta.requiresAuth && !isAuthenticated) {
        next('/login')
    } else if (to.path === '/login' && !isAuthenticated) {
        next('/')
    } else {

        next()
    }

})

export default router


