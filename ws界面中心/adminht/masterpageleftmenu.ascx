<%@ Control Language="C#" AutoEventWireup="true" CodeFile="masterpageleftmenu.ascx.cs" Inherits="masterpageleftmenu" %>
 
<div id="sidebar" class="sidebar                  responsive   ">
    <script type="text/javascript">
        try { ace.settings.check('sidebar', 'fixed') } catch (e) { }
    </script>

    <div class="sidebar-shortcuts" id="sidebar-shortcuts">
        <div class="sidebar-shortcuts-large" id="sidebar-shortcuts-large">
            <button class="btn btn-success" onclick="window.top.location.href='/adminht/demo_home.aspx'">
                <i class="ace-icon fa fa-home"></i>
            </button>

            <!-- #section:basics/sidebar.layout.shortcuts -->
            <div class="btn btn-warning">
               <a href="http://home.gotodk.cn:8090/" target="_blank" class="white">Git</a>
            </div>

            <button class="btn btn-info">
                <a href="http://www.mirrorsea.com" target="_blank" class="white">Sup</a>
            </button>

      

            <button class="btn btn-grey"   id="qiehuancaidanfengge">
                <i class="ace-icon fa fa-bars"   ></i>
            </button>

            <!-- /section:basics/sidebar.layout.shortcuts -->
        </div>

        <div class="sidebar-shortcuts-mini" id="sidebar-shortcuts-mini">
            <span class="btn btn-success"></span>

            <span class="btn btn-info"></span>

            <span class="btn btn-warning"></span>

            <span class="btn btn-grey"></span>
        </div>
    </div>
    <!-- /.sidebar-shortcuts -->
 
    <ul class="nav nav-list" runat="server" id="menuUL">
      
    </ul>
    <!-- /.nav-list -->

    <!-- #section:basics/sidebar.layout.minimize -->
    <div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">
        <i class="ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>
    </div>

    <!-- /section:basics/sidebar.layout.minimize -->
    <script type="text/javascript">
        try { ace.settings.check('sidebar', 'collapsed') } catch (e) { }
    </script>
</div>
