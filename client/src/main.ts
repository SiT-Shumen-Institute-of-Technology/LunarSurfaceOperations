import { createApp } from 'vue'
import axios from 'axios'

import App from './App.vue'
import router from './router'
import { useAuthState } from './utils/globalUtils'

axios.interceptors.request.use(req => {
    const [ isSignedIn ] = useAuthState();
    if (isSignedIn) {
        const token = window.localStorage.getItem('JWT');
        if (req.headers) {
            req.headers['Authorization'] = `Bearer ${token}`; 
        }
    }

    return req;
});

createApp(App).use(router).mount('#app')
