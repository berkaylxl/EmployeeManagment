<template>
  <div class="header">
    <div class="header-img">
      <img src="./assets/e.png"  alt="">
    </div>
    <div class="header-text">
      <div> <h3>Employee Managment Portal</h3>
      <p> By Berkay </p></div>
     
    </div>
    </div>
  <nav class="navbar navbar-expand-sm bg-light navbar-dark m-2">
    <ul class="navbar-nav">
      <li class="nav-item m-1">
        <router-link class="btn btn-light btn-outline-primary" to="/Employee"
          >Employee</router-link
        >
      </li>
      <li class="nav-item m-1">
        <router-link class="btn btn-light btn-outline-primary" to="/Department"
          >Department</router-link
        >
      </li>
    </ul>
  </nav>
  <hr>
  <router-view />
</template>

<style>
.header {
  display: flex;
  justify-content: center;
  align-content: center;
  background:blue;
  color: white;
  padding: 1rem;

}
.header-img{
  display: flex;
  justify-content: center;
}
img{
  width:50%;
}
.header-text{
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
}
</style>
