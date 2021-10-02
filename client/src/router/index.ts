import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import Home from '../views/Home.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
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
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
