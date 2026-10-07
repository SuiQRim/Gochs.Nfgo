const state = {
  formations: [],
  units: [],
  employees: [],
  equipment: [],
  notifications: []
};

const pages = {
  dashboard: ["Сводка", "Текущее состояние формирований НФГО"],
  formations: ["Формирования", "Структура формирований и подразделений"],
  units: ["Подразделения", "Подразделения внутри формирований НФГО"],
  employees: ["Личный состав", "Учёт сотрудников НФГО"],
  equipment: ["Техника", "Оснащение подразделений"],
  notifications: ["Оповещения", "История и регистрация оповещений"]
};

const modalElement = document.getElementById("entityModal");
const entityModal = new bootstrap.Modal(modalElement);
const detailsModal = new bootstrap.Modal(document.getElementById("detailsModal"));
const form = document.getElementById("entityForm");

const employeeFormationFilter = document.getElementById("employeeFormationFilter");
const employeeUnitFilter = document.getElementById("employeeUnitFilter");
const employeeStatusFilter = document.getElementById("employeeStatusFilter");
const equipmentFormationFilter = document.getElementById("equipmentFormationFilter");
const equipmentUnitFilter = document.getElementById("equipmentUnitFilter");
const equipmentConditionFilter = document.getElementById("equipmentConditionFilter");

document.querySelectorAll("[data-page]").forEach(button => {
  button.addEventListener("click", () => showPage(button.dataset.page));
});

const refreshButton = document.getElementById("refreshButton");
refreshButton.addEventListener("click", () => loadAll(true));
document.addEventListener("click", handleAction);
form.addEventListener("submit", submitModal);

employeeFormationFilter.addEventListener("change", () => {
  updateUnitFilter(employeeUnitFilter, employeeFormationFilter.value);
  renderEmployees();
});
employeeUnitFilter.addEventListener("change", renderEmployees);
employeeStatusFilter.addEventListener("change", renderEmployees);

equipmentFormationFilter.addEventListener("change", () => {
  updateUnitFilter(equipmentUnitFilter, equipmentFormationFilter.value);
  renderEquipment();
});
equipmentUnitFilter.addEventListener("change", renderEquipment);
equipmentConditionFilter.addEventListener("change", renderEquipment);

async function api(url, options = {}) {
  const response = await fetch(url, {
    headers: { "Content-Type": "application/json", ...(options.headers || {}) },
    ...options
  });

  if (response.status === 204) return null;

  const data = await response.json().catch(() => null);
  if (!response.ok) throw new Error(data?.detail || data?.title || "Ошибка запроса");
  return data;
}

function showPage(name) {
  document.querySelectorAll(".page").forEach(x => x.classList.remove("active"));
  document.querySelectorAll("[data-page]").forEach(x => x.classList.toggle("active", x.dataset.page === name));
  document.getElementById(`page-${name}`).classList.add("active");
  document.getElementById("pageTitle").textContent = pages[name][0];
  document.getElementById("pageSubtitle").textContent = pages[name][1];
}

async function loadAll(showFeedback = false) {
  if (showFeedback) {
    refreshButton.disabled = true;
    refreshButton.textContent = "Обновление...";
  }

  try {
    const [dashboard, formations, units, employees, equipment, notifications] = await Promise.all([
      api("/api/dashboard"),
      api("/api/formations"),
      api("/api/units"),
      api("/api/employees"),
      api("/api/equipment"),
      api("/api/notifications")
    ]);

    Object.assign(state, { formations, units, employees, equipment, notifications });
    populateFilters();
    renderDashboard(dashboard);
    renderFormations();
    renderUnits();
    renderEmployees();
    renderEquipment();
    renderNotifications();

    if (showFeedback) {
      toast("Данные обновлены");
    }
  } catch (error) {
    toast(error.message, true);
  } finally {
    if (showFeedback) {
      refreshButton.disabled = false;
      refreshButton.textContent = "Обновить";
    }
  }
}

function renderDashboard(d) {
  const metrics = [
    ["Формирования", d.formationCount],
    ["Подразделения", d.unitCount],
    ["Личный состав", d.employeeCount],
    ["Единицы техники", d.equipmentCount]
  ];

  document.getElementById("metrics").innerHTML = metrics.map(([label, value]) => `
    <div class="col-sm-6 col-xl-3">
      <div class="metric-card">
        <div class="metric-label">${label}</div>
        <div class="metric-value">${value}</div>
      </div>
    </div>`).join("");

  document.getElementById("formationStatusSummary").innerHTML = [
    statusRow("Готовы", d.readyFormationCount, "success"),
    statusRow("Требуют внимания", d.requiresAttentionFormationCount, "warning"),
    statusRow("Неактивны", d.inactiveFormationCount, "secondary")
  ].join("");

  document.getElementById("equipmentAttentionSummary").innerHTML = [
    statusRow("Требует ремонта", d.equipmentRequiringRepairCount, "warning"),
    statusRow("Непригодно", d.unusableEquipmentCount, "danger")
  ].join("");
}

function statusRow(label, value, color) {
  return `<div class="status-row"><span>${label}</span><span class="badge text-bg-${color} badge-status">${value}</span></div>`;
}

function badge(value) {
  const map = {
    Ready: ["Готово", "success"], RequiresAttention: ["Требует внимания", "warning"], Inactive: ["Неактивно", "secondary"],
    Active: ["Активен", "success"], Unavailable: ["Недоступен", "warning"], Excluded: ["Исключён", "secondary"],
    Good: ["Исправно", "success"], RequiresRepair: ["Требует ремонта", "warning"], Unusable: ["Непригодно", "danger"],
    Created: ["Создано", "secondary"], Sent: ["Отправлено", "success"], Failed: ["Ошибка", "danger"]
  };
  const item = map[value] || [value, "secondary"];
  return `<span class="badge text-bg-${item[1]} badge-status">${item[0]}</span>`;
}

function unitName(id) {
  return state.units.find(x => x.id === id)?.name || `#${id}`;
}

function formationName(id) {
  return state.formations.find(x => x.id === id)?.name || `#${id}`;
}

function renderFormations() {
  document.getElementById("formationsTable").innerHTML = state.formations.length
    ? state.formations.map(x => `
      <tr>
        <td><strong>${escapeHtml(x.name)}</strong></td>
        <td>${typeLabel(x.type)}</td>
        <td>${escapeHtml(x.leaderName || "—")}</td>
        <td>${escapeHtml(x.location)}</td>
        <td>${badge(x.status)}</td>
        <td><div class="action-buttons">
          <button class="btn btn-sm btn-outline-secondary" data-action="details-formation" data-id="${x.id}">Карточка</button>
          <button class="btn btn-sm btn-outline-primary" data-action="edit-formation" data-id="${x.id}">Изменить</button>
          <button class="btn btn-sm btn-outline-danger" data-action="delete-formation" data-id="${x.id}">Удалить</button>
        </div></td>
      </tr>`).join("")
    : emptyRow(6);
}

function renderUnits() {
  document.getElementById("unitsTable").innerHTML = state.units.length
    ? state.units.map(x => `
      <tr>
        <td><strong>${escapeHtml(x.name)}</strong></td>
        <td>${escapeHtml(formationName(x.formationId))}</td>
        <td>${escapeHtml(x.purpose || "—")}</td>
        <td>${escapeHtml(x.leaderName || "—")}</td>
        <td><div class="action-buttons">
          <button class="btn btn-sm btn-outline-primary" data-action="edit-unit" data-id="${x.id}">Изменить</button>
          <button class="btn btn-sm btn-outline-danger" data-action="delete-unit" data-id="${x.id}">Удалить</button>
        </div></td>
      </tr>`).join("")
    : emptyRow(5);
}

function renderEmployees() {
  const formationId = Number(employeeFormationFilter.value) || null;
  const unitId = Number(employeeUnitFilter.value) || null;
  const status = employeeStatusFilter.value;

  const items = state.employees.filter(x => {
    const unit = state.units.find(u => u.id === x.unitId);
    if (formationId && unit?.formationId !== formationId) return false;
    if (unitId && x.unitId !== unitId) return false;
    if (status && x.status !== status) return false;
    return true;
  });

  document.getElementById("employeesTable").innerHTML = items.length
    ? items.map(x => `
      <tr>
        <td>${escapeHtml(x.personnelNumber)}</td>
        <td><strong>${escapeHtml(x.fullName)}</strong></td>
        <td>${escapeHtml(x.position)}</td>
        <td>${escapeHtml(unitName(x.unitId))}</td>
        <td>${badge(x.status)}</td>
        <td><div class="action-buttons">
          <button class="btn btn-sm btn-outline-primary" data-action="edit-employee" data-id="${x.id}">Изменить</button>
          <button class="btn btn-sm btn-outline-danger" data-action="delete-employee" data-id="${x.id}">Удалить</button>
        </div></td>
      </tr>`).join("")
    : emptyRow(6);
}

function renderEquipment() {
  const formationId = Number(equipmentFormationFilter.value) || null;
  const unitId = Number(equipmentUnitFilter.value) || null;
  const condition = equipmentConditionFilter.value;

  const items = state.equipment.filter(x => {
    const unit = state.units.find(u => u.id === x.unitId);
    if (formationId && unit?.formationId !== formationId) return false;
    if (unitId && x.unitId !== unitId) return false;
    if (condition && x.condition !== condition) return false;
    return true;
  });

  document.getElementById("equipmentTable").innerHTML = items.length
    ? items.map(x => `
      <tr>
        <td><strong>${escapeHtml(x.name)}</strong></td>
        <td>${typeLabel(x.type)}</td>
        <td>${escapeHtml(x.inventoryNumber || "—")}</td>
        <td>${x.quantity}</td>
        <td>${badge(x.condition)}</td>
        <td><div class="action-buttons">
          <button class="btn btn-sm btn-outline-primary" data-action="edit-equipment" data-id="${x.id}">Изменить</button>
          <button class="btn btn-sm btn-outline-danger" data-action="delete-equipment" data-id="${x.id}">Удалить</button>
        </div></td>
      </tr>`).join("")
    : emptyRow(6);
}

function renderNotifications() {
  document.getElementById("notificationsTable").innerHTML = state.notifications.length
    ? state.notifications.map(x => `
      <tr>
        <td>${new Date(x.createdAt).toLocaleString("ru-RU")}</td>
        <td>${escapeHtml(formationName(x.formationId))}</td>
        <td>${escapeHtml(x.message)}</td>
        <td>${escapeHtml(x.createdBy)}</td>
        <td>${badge(x.status)}</td>
        <td><div class="action-buttons">
          ${x.status === "Created" ? `
            <button class="btn btn-sm btn-outline-success" data-action="notification-sent" data-id="${x.id}">Отправлено</button>
            <button class="btn btn-sm btn-outline-danger" data-action="notification-failed" data-id="${x.id}">Ошибка</button>` : ""}
        </div></td>
      </tr>`).join("")
    : emptyRow(6);
}

function emptyRow(columns) {
  return `<tr><td colspan="${columns}" class="empty-row">Нет данных</td></tr>`;
}

function typeLabel(value) {
  const map = {
    Rescue: "Спасательное", Medical: "Медицинское", Engineering: "Инженерное", Fire: "Противопожарное",
    Vehicle: "Транспорт", Communication: "Связь", Other: "Прочее"
  };
  return map[value] || value;
}

function handleAction(event) {
  const button = event.target.closest("[data-action]");
  if (!button) return;
  const action = button.dataset.action;
  const id = Number(button.dataset.id);

  if (action === "details-formation") return showFormationDetails(id);
  if (action.startsWith("create-")) return openEditor(action.slice(7));
  if (action.startsWith("edit-")) return openEditor(action.slice(5), id);
  if (action.startsWith("delete-")) return removeEntity(action.slice(7), id);
  if (action === "notification-sent") return changeNotificationStatus(id, "Sent");
  if (action === "notification-failed") return changeNotificationStatus(id, "Failed");
}

function openEditor(type, id = null) {
  const collection = type === "formation" ? "formations" : type === "unit" ? "units" : type === "employee" ? "employees" : "equipment";
  const entity = id ? state[collection].find(x => x.id === id) : null;
  form.dataset.type = type;
  form.dataset.id = id || "";
  document.getElementById("modalTitle").textContent = `${id ? "Изменить" : "Добавить"}: ${entityTitle(type)}`;
  document.getElementById("modalBody").innerHTML = editorFields(type, entity ?? {});
  entityModal.show();
}

function entityTitle(type) {
  return ({ formation: "формирование", unit: "подразделение", employee: "сотрудник", equipment: "техника", notification: "оповещение" })[type];
}

function editorFields(type, x = {}) {
  if (type === "formation") return `
    ${input("name", "Название", x.name, true)}
    ${select("type", "Тип", ["Rescue","Medical","Engineering","Fire","Other"], x.type, true)}
    ${input("purpose", "Назначение", x.purpose)}
    ${input("location", "Место расположения", x.location, true)}
    ${input("leaderName", "Руководитель", x.leaderName)}
    ${x.id ? select("status", "Статус", ["Ready","RequiresAttention","Inactive"], x.status, true) : ""}`;

  if (type === "unit") return `
    ${selectFrom("formationId", "Формирование", state.formations, x.formationId, true)}
    ${input("name", "Название", x.name, true)}
    ${input("purpose", "Назначение", x.purpose)}
    ${input("leaderName", "Руководитель", x.leaderName)}`;

  if (type === "employee") return `
    ${selectFrom("unitId", "Подразделение", state.units, x.unitId, true)}
    ${input("personnelNumber", "Табельный номер", x.personnelNumber, true)}
    ${input("fullName", "ФИО", x.fullName, true)}
    ${input("position", "Должность", x.position, true)}
    ${input("phone", "Телефон", x.phone)}
    ${x.id ? select("status", "Статус", ["Active","Unavailable","Excluded"], x.status, true) : ""}`;

  if (type === "equipment") return `
    ${selectFrom("unitId", "Подразделение", state.units, x.unitId, true)}
    ${input("name", "Наименование", x.name, true)}
    ${select("type", "Тип", ["Vehicle","Communication","Rescue","Medical","Other"], x.type, true)}
    ${input("inventoryNumber", "Инвентарный номер", x.inventoryNumber)}
    <div class="form-text mb-2">Если указан инвентарный номер, количество должно быть равно 1.</div>
    ${input("quantity", "Количество", x.quantity || 1, true, "number")}
    ${x.id ? select("condition", "Состояние", ["Good","RequiresRepair","Unusable"], x.condition, true) : ""}`;

  return `
    ${selectFrom("formationId", "Формирование", state.formations, "", true)}
    <div class="mb-3"><label class="form-label">Сообщение</label><textarea class="form-control" name="message" required maxlength="1000"></textarea></div>
    ${input("createdBy", "Создал", "", true)}`;
}

function input(name, label, value = "", required = false, type = "text") {
  const min = name === "quantity" ? ' min="1"' : "";
  return `<div class="mb-3"><label class="form-label">${label}</label><input class="form-control" type="${type}" name="${name}" value="${escapeAttr(value ?? "")}"${min} ${required ? "required" : ""}></div>`;
}

function select(name, label, values, selected, required = false) {
  return `<div class="mb-3"><label class="form-label">${label}</label><select class="form-select" name="${name}" ${required ? "required" : ""}>
    <option value="">Выберите...</option>
    ${values.map(v => `<option value="${v}" ${v === selected ? "selected" : ""}>${typeLabel(v)}</option>`).join("")}
  </select></div>`;
}

function selectFrom(name, label, items, selected, required = false) {
  return `<div class="mb-3"><label class="form-label">${label}</label><select class="form-select" name="${name}" ${required ? "required" : ""}>
    <option value="">Выберите...</option>
    ${items.map(v => `<option value="${v.id}" ${v.id === selected ? "selected" : ""}>${escapeHtml(v.name)}</option>`).join("")}
  </select></div>`;
}

async function submitModal(event) {
  event.preventDefault();
  const type = form.dataset.type;
  const id = form.dataset.id;
  const data = Object.fromEntries(new FormData(form).entries());

  ["unitId","formationId","quantity"].forEach(key => {
    if (data[key] !== undefined && data[key] !== "") data[key] = Number(data[key]);
  });
  Object.keys(data).forEach(key => { if (data[key] === "") data[key] = null; });

  const endpoint = type === "formation" ? "/api/formations"
    : type === "unit" ? "/api/units"
    : type === "employee" ? "/api/employees"
    : type === "equipment" ? "/api/equipment"
    : "/api/notifications";

  try {
    await api(id ? `${endpoint}/${id}` : endpoint, {
      method: id ? "PUT" : "POST",
      body: JSON.stringify(data)
    });
    entityModal.hide();
    toast("Изменения сохранены");
    await loadAll();
  } catch (error) {
    toast(error.message, true);
  }
}

async function removeEntity(type, id) {
  if (!confirm("Удалить запись?")) return;
  const endpoint = type === "formation" ? "/api/formations"
    : type === "unit" ? "/api/units"
    : type === "employee" ? "/api/employees"
    : "/api/equipment";

  try {
    await api(`${endpoint}/${id}`, { method: "DELETE" });
    toast("Запись удалена");
    await loadAll();
  } catch (error) {
    toast(error.message, true);
  }
}

async function changeNotificationStatus(id, status) {
  try {
    await api(`/api/notifications/${id}/status`, {
      method: "PATCH",
      body: JSON.stringify({ status })
    });
    toast("Статус оповещения обновлён");
    await loadAll();
  } catch (error) {
    toast(error.message, true);
  }
}

function populateFilters() {
  populateFormationFilter(employeeFormationFilter);
  populateFormationFilter(equipmentFormationFilter);
  updateUnitFilter(employeeUnitFilter, employeeFormationFilter.value);
  updateUnitFilter(equipmentUnitFilter, equipmentFormationFilter.value);
}

function populateFormationFilter(selectElement) {
  const selected = selectElement.value;
  const firstOption = selectElement.options[0]?.outerHTML || '<option value="">Все формирования</option>';
  selectElement.innerHTML = firstOption + state.formations
    .map(x => `<option value="${x.id}">${escapeHtml(x.name)}</option>`)
    .join("");
  if ([...selectElement.options].some(x => x.value === selected)) selectElement.value = selected;
}

function updateUnitFilter(selectElement, formationValue) {
  const selected = selectElement.value;
  const formationId = Number(formationValue) || null;
  const items = formationId
    ? state.units.filter(x => x.formationId === formationId)
    : state.units;

  selectElement.innerHTML = '<option value="">Все подразделения</option>' + items
    .map(x => `<option value="${x.id}">${escapeHtml(x.name)}</option>`)
    .join("");

  if ([...selectElement.options].some(x => x.value === selected)) selectElement.value = selected;
}

async function showFormationDetails(id) {
  try {
    const [formation, employees, equipment, notifications] = await Promise.all([
      api(`/api/formations/${id}`),
      api(`/api/formations/${id}/employees`),
      api(`/api/formations/${id}/equipment`),
      api(`/api/formations/${id}/notifications`)
    ]);

    document.getElementById("detailsModalTitle").textContent = formation.name;
    document.getElementById("detailsModalBody").innerHTML = `
      <div class="details-grid">
        <div class="details-item"><div class="details-label">Тип</div><strong>${typeLabel(formation.type)}</strong></div>
        <div class="details-item"><div class="details-label">Статус</div>${badge(formation.status)}</div>
        <div class="details-item"><div class="details-label">Руководитель</div><strong>${escapeHtml(formation.leaderName || "—")}</strong></div>
        <div class="details-item"><div class="details-label">Место расположения</div><strong>${escapeHtml(formation.location)}</strong></div>
        <div class="details-item"><div class="details-label">Личный состав</div><strong>${formation.employeeCount}</strong></div>
        <div class="details-item"><div class="details-label">Единицы техники/имущества</div><strong>${formation.equipmentCount}</strong></div>
      </div>
      <div class="details-section">
        <h3 class="h6">Назначение</h3>
        <p class="mb-0">${escapeHtml(formation.purpose || "Не указано")}</p>
      </div>
      <div class="details-section">
        <h3 class="h6">Подразделения</h3>
        ${simpleTable(["Название", "Назначение", "Руководитель"], formation.units.map(x => [
          escapeHtml(x.name), escapeHtml(x.purpose || "—"), escapeHtml(x.leaderName || "—")
        ]))}
      </div>
      <div class="details-section">
        <h3 class="h6">Личный состав</h3>
        ${simpleTable(["Табельный №", "ФИО", "Должность", "Статус"], employees.map(x => [
          escapeHtml(x.personnelNumber), escapeHtml(x.fullName), escapeHtml(x.position), badge(x.status)
        ]))}
      </div>
      <div class="details-section">
        <h3 class="h6">Техника и имущество</h3>
        ${simpleTable(["Наименование", "Инв. №", "Количество", "Состояние"], equipment.map(x => [
          escapeHtml(x.name), escapeHtml(x.inventoryNumber || "—"), x.quantity, badge(x.condition)
        ]))}
      </div>
      <div class="details-section">
        <h3 class="h6">Оповещения</h3>
        ${simpleTable(["Дата", "Сообщение", "Статус"], notifications.map(x => [
          new Date(x.createdAt).toLocaleString("ru-RU"), escapeHtml(x.message), badge(x.status)
        ]))}
      </div>
    `;

    detailsModal.show();
  } catch (error) {
    toast(error.message, true);
  }
}

function simpleTable(headers, rows) {
  if (!rows.length) return '<div class="text-secondary small">Нет данных</div>';

  return `
    <div class="table-responsive">
      <table class="table table-sm align-middle">
        <thead><tr>${headers.map(x => `<th>${x}</th>`).join("")}</tr></thead>
        <tbody>
          ${rows.map(row => `<tr>${row.map(cell => `<td>${cell}</td>`).join("")}</tr>`).join("")}
        </tbody>
      </table>
    </div>`;
}

function toast(message, error = false) {
  const element = document.createElement("div");
  element.className = `toast align-items-center text-bg-${error ? "danger" : "success"} border-0`;
  element.innerHTML = `<div class="d-flex"><div class="toast-body">${escapeHtml(message)}</div><button class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>`;
  document.getElementById("toastContainer").appendChild(element);
  const instance = new bootstrap.Toast(element, { delay: 3000 });
  instance.show();
  element.addEventListener("hidden.bs.toast", () => element.remove());
}

function escapeHtml(value) {
  return String(value ?? "").replace(/[&<>"']/g, c => ({ "&":"&amp;", "<":"&lt;", ">":"&gt;", '"':"&quot;", "'":"&#039;" })[c]);
}

function escapeAttr(value) {
  return escapeHtml(value);
}

loadAll();
