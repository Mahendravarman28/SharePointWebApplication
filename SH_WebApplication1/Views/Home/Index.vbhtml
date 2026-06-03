@Code
    ViewData("Title") = "Platform Home"
End Code

<div class="container-fluid mt-4">
    <div class="row mb-4">
        <div class="col-12">
            <div class="p-4 bg-primary text-white rounded shadow-sm">
                <h1 class="display-5 fw-bold"><i class="bi bi-grid-3x3-gap-fill me-2"></i>SharePoint-Like Platform</h1>
                <p class="lead mb-3">A metadata-driven list management platform built with ASP.NET MVC 5 + VB.NET + .NET 4.8</p>
                @Html.ActionLink("Get Started → Create a List", "Create", "ListBuilder", Nothing, New With {.class = "btn btn-light btn-lg"})
            </div>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-md-4">
            <div class="card h-100 shadow-sm border-0">
                <div class="card-body text-center p-4">
                    <i class="bi bi-table fs-1 text-primary mb-3 d-block"></i>
                    <h5 class="fw-bold">Module A — List Builder</h5>
                    <p class="text-muted">Create dynamic lists with custom fields. Define columns, types, validation, and metadata — no SQL required.</p>
                    @Html.ActionLink("Open List Builder", "Index", "ListBuilder", Nothing, New With {.class = "btn btn-outline-primary"})
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card h-100 shadow-sm border-0">
                <div class="card-body text-center p-4">
                    <i class="bi bi-ui-checks-grid fs-1 text-success mb-3 d-block"></i>
                    <h5 class="fw-bold">Module B — Form Designer</h5>
                    <p class="text-muted">Design New, Edit, and Display forms with drag-and-drop. Set conditional rules and publish for runtime use.</p>
                    @Html.ActionLink("Go to Lists", "Index", "ListBuilder", Nothing, New With {.class = "btn btn-outline-success"})
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card h-100 shadow-sm border-0">
                <div class="card-body text-center p-4">
                    <i class="bi bi-grid-3x3-gap fs-1 text-info mb-3 d-block"></i>
                    <h5 class="fw-bold">Module C — View Designer</h5>
                    <p class="text-muted">Configure grid, card, kanban, and grouped views with filters, sorting, and conditional formatting.</p>
                    @Html.ActionLink("Go to Lists", "Index", "ListBuilder", Nothing, New With {.class = "btn btn-outline-info"})
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card h-100 shadow-sm border-0">
                <div class="card-body text-center p-4">
                    <i class="bi bi-layout-wtf fs-1 text-warning mb-3 d-block"></i>
                    <h5 class="fw-bold">Module D — Home Page Designer</h5>
                    <p class="text-muted">Build a custom home page for each list with KPI tiles, charts, recent items, and action widgets.</p>
                    @Html.ActionLink("Go to Lists", "Index", "ListBuilder", Nothing, New With {.class = "btn btn-outline-warning"})
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card h-100 shadow-sm border-0">
                <div class="card-body text-center p-4">
                    <i class="bi bi-diagram-3-fill fs-1 text-danger mb-3 d-block"></i>
                    <h5 class="fw-bold">Module E — Workflow Engine</h5>
                    <p class="text-muted">Design state-machine workflows with approval chains, conditions, and automated transitions.</p>
                    @Html.ActionLink("Go to Lists", "Index", "ListBuilder", Nothing, New With {.class = "btn btn-outline-danger"})
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card h-100 shadow-sm border-0">
                <div class="card-body text-center p-4">
                    <i class="bi bi-shield-lock-fill fs-1 text-dark mb-3 d-block"></i>
                    <h5 class="fw-bold">Module F — Security / RBAC</h5>
                    <p class="text-muted">Manage roles, permissions, and user assignments at application, list, field, and action level.</p>
                    @Html.ActionLink("Open Security", "Roles", "Security", Nothing, New With {.class = "btn btn-outline-dark"})
                </div>
            </div>
        </div>
    </div>
</div>
