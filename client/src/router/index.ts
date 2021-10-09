import { useAuthState } from '@/composables/state/globalState';
import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'

function authRestrict (to: any, from: any, next: any) {
      const {isSignedIn} = useAuthState();
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
      name: 'Workspace_add',
      component: () => import('../views/Workspace_add.vue'),
      beforeEnter: authRestrict
  },
  {
      path: '/workspace/:id',
      name: 'Workspace',
      component: () => import('../views/Workspace.vue'),
      beforeEnter: authRestrict
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
