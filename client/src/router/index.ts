import { useAuthState } from '@/utils/globalUtils'
import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'

function authRestrict (to: any, from: any, next: any) {
      const [isSignedIn] = useAuthState();
      if(!isSignedIn.value) {
          next({
              path: '/signin'
          });
      } else {
          next();
      }
}

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
      component: () => import('../views/Home.vue')
  },
  {
      path: '/register',
      name: 'Register',
      component: () => import('../views/Register.vue')
  },
  {
      path: '/signin',
      name: 'Signin',
      component: () => import('../views/SignIn.vue')
  },
  {
      path: '/workspace_create',
      name: 'Create workspace',
      component: () => import('../views/Workspace_add.vue'),
      beforeEnter: authRestrict
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
