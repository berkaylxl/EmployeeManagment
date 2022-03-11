<template>
  <button
    class="btn btn-primary mb-3 mr-3"
    style="float: right"
    data-bs-toggle="modal"
    data-bs-target="#exampleModal"
    @click="addClick"
  >
    <i class="fa-solid fa-square-plus"></i>
    Add Employee
  </button>
  <div class="d-flex col-4 mb-1">
          <input type="text"
          @keyup="filter"
          class="form-control"
          placeholder="Filter"
          v-model="EmployeeNameFilter"
          >
          </div>
  <table class="table table-striped">
    <thead>
      <tr>
        <th>Employee Id</th>
        <th>Employee Name</th>
        <th>Department</th>
        <th>Date of Joining</th>
        <th>Options</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="emp in employees" :key="emp">
        <td>{{ emp.EmployeeId }}</td>
        <td>{{ emp.EmployeeName }}</td>
        <td>{{ emp.Department }}</td>
        <td>{{ emp.DateOfJoining }}</td>
        <td>
          <button
            class="btn btn-outline-dark"
            data-bs-toggle="modal"
            data-bs-target="#exampleModal"
            @click="editClick(emp)"
          >
            <i class="fa-solid fa-pen-to-square"></i>
          </button>
          <button
            class="btn btn-outline-danger ml-1"
            @click="deleteClick(emp.EmployeeId)"
          >
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
          <div class="d-flex flex-row bd-highlight mb-3">
            <div class="p-2 w-50 bd-highlight mr-3">
              <div class="input-group mb-3">
                <span class="input-group-text mr-1"> Name</span>
                <input
                  type="text"
                  class="form-control"
                  v-model="EmployeeName"
                />
              </div>

              <div class="input-group mb-3">
                <span class="input-group-text mr-1">Department</span>
                <select class="form-select" v-model="Department">
                  <option v-for="dep in departments" :key="dep">
                    {{ dep.DepartmentName }}
                  </option>
                </select>
              </div>

              <div class="input-group mb-1">
                <span class="input-group-text mr-1">DOJ</span>
                <input
                  type="date"
                  class="form-control"
                  v-model="DateOfJoining"
                />
              </div>
            </div>
            <div class="d-flex  justify-content-center w-50 bd-highlight">
              <img
                style="width: 250px; height: 250px"
                :src="PhotoPath + PhotoFileName"
                alt="Not Found"
              />
            </div>
          </div>
          <input type="file" class="mb-3" @change="imageUpload" />

          <button
            v-if="EmployeeId == 0"
            class="btn btn-primary"
            @click="createClick"
            style="float: right"
          >
            <i class="fa-solid fa-square-plus"></i> Create
          </button>
          <button
            v-if="EmployeeId != 0"
            class="btn btn-primary"
            @click="updateClick"
          >
            <i class="fa-solid fa-square-pen"></i> Update
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
const api_URL = "http://localhost:58064/api/employee";
const photo_URL = "http://localhost:58064/photos/";
import axios from "axios";
export default {
  data() {
    return {
      employees: [],
      departments: [],
      modalTitle: "",
      EmployeeId: 0,
      EmployeeName: "",
      Department: "",
      DateOfJoining: "",
      PhotoFileName: "anonymous.png",
      PhotoPath: photo_URL,
      EmployeeNameFilter:"",
      employeesWithoutFilter:[]
    };
  },
  methods: {
    refreshData() {
      axios.get(api_URL).then((response) => {
        this.employees = response.data;
        this.employeesWithoutFilter=response.data
      });
      axios.get("http://localhost:58064/api/department").then((response) => {
        this.departments = response.data;
      });
    },
    addClick() {
      this.modalTitle = "Add Employee";
      this.EmployeeId = 0;
      this.EmployeeName = "";
      this.Department = "";
      this.DateOfJoining = "";
      this.PhotoFileName = "anonymous.png";
    },
    editClick(emp) {
      this.modalTitle = "Edit Employee";
      this.EmployeeId = emp.EmployeeId;
      this.EmployeeName = emp.EmployeeName;
      this.Department = emp.Department;
      this.DateOfJoining = emp.DateOfJoining;
      this.PhotoFileName = emp.PhotoFileName;
    },
    createClick() {
      axios
        .post(api_URL, {
          EmployeeName: this.EmployeeName,
          Department: this.Department,
          DateOfJoining: this.DateOfJoining,
          PhotoFileName: this.PhotoFileName,
        })
        .then((response) => {
          alert(response.data);
          this.refreshData();
        });
    },
    updateClick() {
      axios
        .put(api_URL, {
          EmployeeId: this.EmployeeId,
          EmployeeName: this.EmployeeName,
          Department: this.Department,
          DateOfJoining: this.DateOfJoining,
          PhotoFileName: this.PhotoFileName,
        })
        .then((response) => {
          alert(response.data);
          this.refreshData();
        });
    },
    deleteClick(id) {
      if (!confirm("Are you sure?")) {
        return;
      }
      axios.delete(api_URL + "?id=" + id).then((response) => {
        this.refreshData();
        alert(response.data);
      });
    },
    imageUpload(event) {
      let formData = new FormData();
      formData.append("file", event.target.files[0]);
      axios.post(api_URL + "/savefile", formData).then((response) => {
        this.PhotoFileName = response.data;
        console.log(this.PhotoFileName);
      });
    },
    filter(){
      var EmployeeNameFilter=this.EmployeeNameFilter
      this.employees=this.employeesWithoutFilter.filter(
        function (e) {
          return e.EmployeeName.toString().toLowerCase().includes(
             EmployeeNameFilter.toString().trim().toLowerCase()
          )
         
        }
      )
    }
  },
  mounted() {
    this.refreshData();
  },
};
</script>

<style>
</style>