import { createApp } from 'vue'
import App from './App.vue'
import router from '../router'
import 'bootstrap/dist/css/bootstrap.min.css'
import ToastPlugin from 'vue-toast-notification'
import 'vue-toast-notification/dist/theme-bootstrap.css'
// import axios from 'axios'

// axios.interceptors.response.use(
//     responce => responce,
//     error => {
//         console.log(error)
//         console.log(error.responce)

//         if(error.responce?.status === 401) {
            
//         } else {
            
//         }
//         return Promise.reject(error)
//     }
// )

const app = createApp(App)

app.use(ToastPlugin);

app.use(router)
app.mount('#app')
