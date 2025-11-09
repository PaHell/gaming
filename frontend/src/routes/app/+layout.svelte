<script lang="ts">
	import * as Sidebar from '$lib/shadcn/components/ui/sidebar/index.js';
	import AppSidebar from '$lib/shadcn/components/app-sidebar.svelte';
</script>

<Sidebar.Provider
	style="--sidebar-width: calc(var(--spacing) * 72); --header-height: calc(var(--spacing) * 12);"
>
	<AppSidebar variant="inset" />
	<Sidebar.Inset>
		<slot />
	</Sidebar.Inset>
</Sidebar.Provider>
