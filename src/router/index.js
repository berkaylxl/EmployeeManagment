import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
  {
    path:"/Employee",
    component:()=>import('../views/Employee.vue')
  },
  {
    path:"/Department",
    component:()=>import('../views/Department.vue')
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
