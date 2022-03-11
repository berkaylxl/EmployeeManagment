<template>
  <button
    class="btn btn-primary m-2"
    style="float: right"
    data-bs-toggle="modal"
    data-bs-target="#exampleModal"
    @click="addClick"
  >
    <i class="fa-solid fa-square-plus"></i>
    Add Department
  </button>
  <table class="table table-striped">
    <thead>
      <tr>
        <th>Department Id</th>
        <th>Department Name</th>
        <th>Options</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="dep in departments" :key="dep">
        <td>{{ dep.DepartmentId }}</td>
        <td>{{ dep.DepartmentName }}</td>
        <td>
          <button
            class="btn btn-outline-dark"
            data-bs-toggle="modal"
            data-bs-target="#exampleModal"
            @click="editClick(dep)"
          >
            <i class="fa-solid fa-pen-to-square"></i>
          </button>
          <button class="btn btn-outline-danger ml-1"
          @click="deleteClick(dep.DepartmentId)">
            <i class="fa-solid fa-trash-can"></i>
          </button>
        </td>
      </tr>
    </tbody>
  </table>
  <div
    class="modal fade"
    id="exampleModal"
    tabindex="-1"
    aria-labelledby="modalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalLabel">{{ modalTitle }}</h5>
          <button
            style="border-radius: 50% 50%"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          >
            <i class="fa-solid fa-circle-xmark"></i>
          </button>
        </div>
        <div class="modal-body">
          <div class="input-group mb-3">
            <span class="input-group-text mr-1">Department Name</span>
            <input type="text" class="form-control" v-model="departmentName" />
          </div>
          <button v-if="departmentId == 0" 
          class="btn btn-primary"
          @click="createClick">
            <i class="fa-solid fa-square-plus"></i> Create
          </button>
          <button v-if="departmentId != 0"
           class="btn btn-primary"
           @click="updateClick">
            <i class="fa-solid fa-square-pen"></i> Update
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
const api_URL = "http://localhost:58064/api/department";
import axios from "axios";
export default {
  data() {
    return {
      departments: [],
      modalTitle: "",
      departmentName: "",
      departmentId: 0,
    };
  },
  methods: {
    refreshData() {
      axios.get(api_URL).then((response) => {
        this.departments = response.data;
      });
    },
    addClick() {
      this.modalTitle = "Add Department";
      this.departmentId = 0;
      this.departmentName = "";
    },
    editClick(dep) {
      this.modalTitle = "Edit Department";
      this.departmentId = dep.DepartmentId;
      this.departmentName = dep.DepartmentName;
    },
    createClick(){
        axios.post(api_URL,{
          DepartmentName:this.departmentName
        })
        .then((response) => {
        alert(response.data)
        this.refreshData()
      });
    },
    updateClick(){
       axios.put(api_URL,{
          DepartmentId:this.departmentId,
          DepartmentName:this.departmentName
        })
        .then((response) => {
        alert(response.data)
        this.refreshData()
      });
    },
    deleteClick(id){
     
      if (!confirm("Are you sure?")) {
        return
      }
      axios.delete(api_URL+"?id="+id)
      .then(response => {
          this.refreshData()
          alert(response.data)
      })
  },
  },
  mounted() {
    this.refreshData();
  },
};
</script>

<style>
</style>