import CreateAccount from "../views/createAccount.vue"
// import AuthService from "../services/authService"
import AuthView from "../views/authView.vue"
import HomeView from "../views/homeView.vue"
import TransactionView from "../views/transactionView.vue"
import ReportView from "../views/reportView.vue"
import { createRouter, createWebHashHistory } from "vue-router"
// import axios from "axios"

const routes = [
    {
        path: '/login', 
        component: AuthView, 
        meta: {requiresAuth: false}
    },
    {
        path: '/', 
        component: HomeView,
        meta: {requiresAuth: true}
    },
    {path: '/create', component: CreateAccount, meta: {requiresAuth: false}},
    {path: '/transaction', component: TransactionView, meta: {requiresAuth: true}},
    {path: '/report', component: ReportView, meta: {requiresAuth: true}},
]

const router = createRouter({history: createWebHashHistory(), routes})

router.beforeEach(async (to, from, next) => {
    const token = localStorage.getItem('token');
    if(to.matched.some((route) => route.meta.requiresAuth)) {
        if(token) {
            next()
        } else {
            next('/login')
        }
    } else {
        next()
    }
})

export default router


