import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import 'bootstrap/dist/css/bootstrap.min.css'
import ToastPlugin from 'vue-toast-notification'
import 'vue-toast-notification/dist/theme-bootstrap.css'
import './api/interceptors'

const app = createApp(App)

app.use(ToastPlugin);

app.use(router)
app.mount('#app')
