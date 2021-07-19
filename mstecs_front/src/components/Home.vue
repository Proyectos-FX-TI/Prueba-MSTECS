<template>
  <div>
    <h1>Bienvenido: {{ account.user.nombre }}</h1>
    <em v-if="users.loading">Cargando usuarios...</em>
    <span v-if="users.error" class="text-danger">ERROR: {{ users.error }}</span>
    <div class="form-group">
      <button class="btn btn-primary" v-on:click="exportar">
        Exportar PDF
      </button>
    </div>
    <ul v-if="users.items">
      <li v-for="user in users.items" :key="user.id_usuario">
        {{ user.nombre + " - " + user.rol }}
      </li>
    </ul>
    <p>
      <router-link to="/login">Cerrar Sesión</router-link>
    </p>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
  computed: {
    ...mapState({
      account: (state) => state.account,
      users: (state) => state.users.all,
    }),
  },
  created() {
    this.getAllUsers();
  },
  methods: {
    ...mapActions("users", {
      getAllUsers: "getAll",
    }),
    ...mapActions("account", ["exportpdf"]),
    exportar() {
      this.exportpdf();
    },
  },
};
</script>
