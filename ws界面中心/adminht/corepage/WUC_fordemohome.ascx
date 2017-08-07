<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WUC_fordemohome.ascx.cs" Inherits="adminht_corepage_WUC_fordemohome" %>

 


<div class="row">
							<div class="col-xs-12 col-sm-12">

 
                            

                                <div class="row">

                                    <div class="col-sm-12">
                                       
										<div class="widget-box transparent">
											<div class="widget-header widget-header-flat">
												<h4 class="widget-title lighter">
													<i class="ace-icon fa fa-signal"></i>
													我的工作台
												</h4>

												<div class="widget-toolbar">
													<a href="#" data-action="collapse">
														<i class="ace-icon fa fa-chevron-up"></i>
													</a>
												</div>
											</div>

											<div class="widget-body">
												<div class="widget-main padding-4">
													 


                                                    <div class="space-12"></div>

                                                    <div class="row">
									                  
                                                        <%
                                                            if (dtgzt != null)
                                                            {
                                                                for (int i = 0; i < dtgzt.Rows.Count; i++)
                                                                {
                                                                    if (i < 4)
                                                                    {
                                                                    %>
                                                           <div class="col-sm-3">
									  <div class="well">
											<h4 class="green smaller lighter"><a class="red" href="<%=dtgzt.Rows[i]["Mdizhi"].ToString() %>"><%=dtgzt.Rows[i]["Mbiaoti"].ToString() %></a><a href="/adminht/corepage/bas/edit_mygzt.aspx?idforedit=gzt_<%=UserSession.唯一键%>_<%=i.ToString() %>&fff=1"><i id="gzt_<%=UserSession.唯一键%>_<%=i.ToString() %>" class="ace-icon fa fa-pencil-square-o align-top bigger-125 pull-right inline" style="cursor:pointer"></i></a></h4>
											<%=dtgzt.Rows[i]["Mbeiwanglu"].ToString() %>
										</div>
									</div>

                                                        <%
                                                                    }
                                                                }
                                                            }
                                                             %>
                                                               
								</div> <div class="row">
									                  
                                               <%
                                                            if (dtgzt != null)
                                                            {
                                                                for (int i = 0; i < dtgzt.Rows.Count; i++)
                                                                {
                                                                    if (i > 3)
                                                                    {
                                                                    %>
                                                           <div class="col-sm-3">
									  <div class="well">
											<h4 class="green smaller lighter"><a class="orange" href="<%=dtgzt.Rows[i]["Mdizhi"].ToString() %>"><%=dtgzt.Rows[i]["Mbiaoti"].ToString() %></a><a href="/adminht/corepage/bas/edit_mygzt.aspx?idforedit=gzt_<%=UserSession.唯一键%>_<%=i.ToString() %>&fff=1"><i id="gzt_<%=UserSession.唯一键%>_<%=i.ToString() %>" class="ace-icon fa fa-pencil-square-o align-top bigger-125 pull-right inline" style="cursor:pointer"></i></a></h4>
											<%=dtgzt.Rows[i]["Mbeiwanglu"].ToString() %>
										</div>
									</div>

                                                        <%
                                                                    }
                                                                }
                                                            }
                                                             %>                
								</div>




												</div><!-- /.widget-main -->
											</div><!-- /.widget-body -->
										</div><!-- /.widget-box -->


                                         <div class="center"><hr /> </div>
									</div><!-- /.col -->
								</div><!-- /.row -->





								<!-- PAGE CONTENT ENDS -->
							</div><!-- /.col -->
						</div><!-- /.row -->






