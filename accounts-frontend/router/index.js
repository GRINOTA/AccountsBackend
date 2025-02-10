import AuthService from "../services/authService"
import AuthView from "../views/AuthView.vue"
import HomeView from "../views/HomeView.vue"
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


