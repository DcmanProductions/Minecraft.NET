package chase.minecraft.ForgeWrapper.installer.json;

import java.io.File;

public class InstallV1 extends Install {
  protected String serverJarPath;
  
  public InstallV1(Install v0) {
    this.profile = v0.profile;
    this.version = v0.version;
    this.icon = v0.icon;
    this.minecraft = v0.minecraft;
    this.json = v0.json;
    this.logo = v0.logo;
    this.path = v0.path;
    this.urlIcon = v0.urlIcon;
    this.welcome = v0.welcome;
    this.mirrorList = v0.mirrorList;
    this.hideClient = v0.hideClient;
    this.hideServer = v0.hideServer;
    this.hideExtract = v0.hideExtract;
    this.libraries = v0.libraries;
    this.processors = v0.processors;
    this.data = v0.data;
  }
  
  public String getServerJarPath() {
    if (this.serverJarPath == null)
      return "{ROOT}/minecraft_server.{MINECRAFT_VERSION}.jar"; 
    return this.serverJarPath;
  }
}
